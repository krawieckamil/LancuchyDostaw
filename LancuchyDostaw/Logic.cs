using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LancuchyDostaw
{
    class Logic
    {
        public List<Supplier> suppliers { get; set; }
        public List<Customer> customers { get; set; }
        public TransportConnection[,] connections { get; set; }

        private double[] vectorA;
        private double[] vectorB;
        private double[,] matrixD;
        private double demand, supply;

        private double summaryCosts = 0.0;
        private int suppliersAmount;
        private int customersAmount;

        public Logic()
        {
            suppliers = new List<Supplier>();
            customers = new List<Customer>();
            //            FileHandler.LoadData("InitialData.txt", Providers, Recipients);


            suppliers.Add(new Supplier(1, 0));
            suppliers.Add(new Supplier(1, 0));
            suppliers.Add(new Supplier(1, 0));

            customers.Add(new Customer(2, 0));
            customers.Add(new Customer(1, 0));
            customers.Add(new Customer(1, 0));

            TransportConnection[,] connections = new TransportConnection[3, 3];
            double[,] connectionCosts = new double[3, 3];

            connectionCosts[0, 0] = 3;
            connectionCosts[0, 1] = 7;
            connectionCosts[0, 2] = 6;
            connectionCosts[1, 0] = 5;
            connectionCosts[1, 1] = 6;
            connectionCosts[1, 2] = 2;
            connectionCosts[2, 0] = 2;
            connectionCosts[2, 1] = 5;
            connectionCosts[2, 2] = 4;

            for (int i = 0; i < suppliers.Count; i++)
            {
                for (int j = 0; j < customers.Count; j++)
                {
                    connections[i, j] = new TransportConnection(suppliers[i], customers[j], connectionCosts[i, j], i, j);
                }
            }
            Solve();
        }
        private void init()
        {
            demand = 0.0;
            supply = 0.0;
            foreach (Supplier s in suppliers)
            {
                supply += s.Supply;
            }
            foreach (Customer c in customers)
            {
                demand += c.Demand;
            }
            if (demand < supply)
            {
                Customer cust = new Customer(supply - demand, 0);
                cust.IsFake = true;
                customers.Add(cust);
            }
            else if (supply < demand)
            {
                Supplier supp = new Supplier(demand - supply, 0);
                supp.IsFake = true;
                suppliers.Add(supp);
            }
            customersAmount = customers.Count;
            suppliersAmount = suppliers.Count;
            if (demand != supply)
            {
                TransportConnection[,] tmp = new TransportConnection[suppliersAmount, customersAmount];
                for (int i = 0; i < suppliersAmount; i++)
                {
                    for (int j = 0; j < customersAmount; j++)
                    {
                        if (i < connections.Length && j < connections.Rank)
                        {
                            tmp[i, j] = connections[i, j];
                        }
                        else
                        {
                            tmp[i, j] = new TransportConnection(suppliers[i], customers[j], 0, i, j);
                        }
                        if (tmp[i, j].Supplier.IsFake && tmp[i, j].Customer.IsPrivileged
                                || tmp[i, j].Customer.IsFake && tmp[i, j].Supplier.IsPrivileged)
                        {
                            tmp[i, j].Blocked = true;
                        }
                    }
                }
                connections = tmp;
            }
            matrixD = new double[suppliersAmount, customersAmount];
            vectorA = new double[suppliersAmount];
            vectorB = new double[customersAmount];
        }

        public List<Row> CreateRows()
        {
            List<Row> listOfRows = new List<Row>();
            Row row = new Row();
            row.ValueList.Add("");
            foreach (var recipient in customers)
            {
                row.ValueList.Add("Odbiorca: " + "\nAktualny popyt: " + recipient.Demand + " (" + recipient.Demand + ")");
            }
            row.ValueList.Add("");
            listOfRows.Add(row);
            int i = 0;
            foreach (var provider in suppliers)
            {
                row = new Row();
                row.ValueList.Add("Dostawca: " + provider.Id + "\nPodaż: " + provider.Connections + " (" + provider.Supply + ")");
                foreach (var recipient in customers)
                {
                    row.ValueList.Add("Koszt: " + recipient.Connections[i].TransportCost + " (" + recipient.Connections[i].UnitProfit() + ") ");
                }
                listOfRows.Add(row);
                i++;
            }
            row = new Row();
            row.ValueList.Add("");
            listOfRows.Add(row);
            return listOfRows;
        }

        public void Solve()
        {
            init();
            Utils.drawCostsMatrix(connections);


            Utils.resetAmounts(connections);
            naiveSolution();

            int iter = 0;
            Point minP;
            //double a0 = -connectionCosts[minP.x , minP.y];

            while (true)
            {
                iter++;
                Console.WriteLine("\nIteracja: " + iter);
                Utils.drawAmountMatrix(connections);
                summaryCosts = Utils.countTotalCosts(connections);
                Console.WriteLine("K" + iter + ": " + summaryCosts);
                recalculateABVectors(0.0);
                recalculateMatrixD();
                minP = Utils.getMaximumFromMatrix(matrixD);
                // Console.WriteLine(vectorA);
                // Console.WriteLine(vectorB);
                Utils.drawMatrix(matrixD);
                LinkedList<TransportConnection> relocatePath = findBestRelocationPath(connections[minP.X, minP.Y]);
                double pathProfit = countTotalProfitForRelocationPath(relocatePath);
                Console.WriteLine(pathProfit);
                if (pathProfit > 0.0)
                {
                    try
                    {
                        relocateSuppliesByRelocationPath(relocatePath);
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                summaryCosts = Utils.countTotalCosts(connections);
            }
        }

        private void naiveSolution()
        {
            List<TransportConnection> orderedConList = Utils.getDescendingOrderByProfit(connections);
            foreach (TransportConnection con in orderedConList)
            {
                double Amount = Math.Min((con.Supplier.Supply - Utils.getAmountOfRow(connections, con.X)), (con.Customer.Demand - Utils.getAmountOfColumn(connections, con.Y)));
                con.Amount = Amount;
            };
        }

        private void recalculateMatrixD()
        {
            for (int i = 0; i < suppliersAmount; i++)
            {
                for (int j = 0; j < customersAmount; j++)
                {
                    if (!connections[i, j].Blocked && connections[i, j].Amount == 0)
                    {
                        matrixD[i, j] = connections[i, j].UnitProfit() - vectorA[i] - vectorB[j];
                    }
                    else
                    {
                        matrixD[i, j] = 0;
                    }
                }
            }
        }

        private void recalculateABVectors(double a0)
        {
            Boolean[] ba = new Boolean[vectorA.Length];
            Boolean[] bb = new Boolean[vectorB.Length];
            vectorA[0] = a0;
            ba[0] = true;
            int k = 1, k_prev;
            while (true)
            {
                k_prev = k;
                for (int i = 0; i < vectorA.Length; i++)
                {
                    if (!ba[i])
                    {
                        for (int j = 0; j < vectorB.Length; j++)
                        {
                            if (bb[j] && connections[i, j].Amount > 0)
                            {
                                vectorA[i] = connections[i, j].UnitProfit() - vectorB[j];
                                ba[i] = true;
                                k++;
                            }
                        }
                    }
                    if (ba[i])
                    {
                        for (int j = 0; j < vectorB.Length; j++)
                        {
                            if (!bb[j] && connections[i, j].Amount > 0)
                            {
                                vectorB[j] = connections[i, j].UnitProfit() - vectorA[i];
                                bb[j] = true;
                                k++;
                            }
                        }
                    }
                }
                if (k_prev == k)
                {
                    for (int i = 0; i < vectorA.Length; i++)
                    {
                        if (!ba[i])
                        {
                            vectorA[i] = a0;
                            ba[i] = true;
                            k++;
                            break;
                        }
                    }
                }
                if (k_prev == k)
                {
                    break;
                }
            }
        }

        /**
         * GETTERS AND SETTERS
         */
        public List<Supplier> getSuppliers()
        {
            return suppliers;
        }

        public void setSuppliers(List<Supplier> suppliers)
        {
            this.suppliers = suppliers;
        }

        public List<Customer> getCustomers()
        {
            return customers;
        }

        public void setCustomers(List<Customer> customers)
        {
            this.customers = customers;
        }

        public TransportConnection[,] getConnectionCosts()
        {
            return connections;
        }

        public void setConnections(TransportConnection[,] connections)
        {
            this.connections = connections;
        }

        public double getSummaryCosts()
        {
            return summaryCosts;
        }

        private LinkedList<TransportConnection> findBestRelocationPath(TransportConnection start)
        {
            LinkedList<TransportConnection> list = new LinkedList<TransportConnection>();
            return findBestLoopedRelocationPath(start, list, true, true);
        }

        private LinkedList<TransportConnection> findBestLoopedRelocationPath(TransportConnection currentConn, LinkedList<TransportConnection> listMain, bool verticalDirection, bool nextNeedSupply)
        {
            listMain.AddLast(currentConn);
            List<TransportConnection> toCheck = null;
            if (verticalDirection)
            {
                toCheck = Utils.getColumnAsList(currentConn.X, connections);
            }
            else
            {
                toCheck = Utils.getRowAsList(currentConn.Y, connections);
            }
            if (currentConn != listMain.First() && toCheck.Contains(listMain.First()) && !nextNeedSupply)
            {
                return listMain;
            }

            LinkedList<TransportConnection> bestList = null, tmpList = null;
            foreach (TransportConnection c in toCheck)
            {
                if (c != currentConn && !listMain.Contains(c) && !c.Blocked)
                {
                    if (!nextNeedSupply || c.Amount > 0)
                    {
                        LinkedList<TransportConnection> tmp = new LinkedList<TransportConnection>(listMain);
                        tmpList = findBestLoopedRelocationPath(c, tmp,
                                !verticalDirection, !nextNeedSupply);
                        if (tmpList != null && (bestList == null || countTotalProfitForRelocationPath(tmpList) > countTotalProfitForRelocationPath(bestList)))
                        {
                            bestList = tmpList;
                        }
                    }
                }
            }
            return bestList;
        }

        private double countTotalProfitForRelocationPath(LinkedList<TransportConnection> tmpList)
        {
            double AmountToMove = countAmountToMoveForRelocationPath(tmpList);
            double UnitProfit = countUnitProfitForRelocationPath(tmpList);
            return UnitProfit * AmountToMove;
        }

        private double countAmountToMoveForRelocationPath(LinkedList<TransportConnection> tmpList)
        {
            double AmountToMove = tmpList.ElementAt(1).Amount;            
            for (int i = 1; i<tmpList.Count; i++)
            {
                TransportConnection c = tmpList.ElementAt(i);
                if (i % 2 == 0)
                {
                    AmountToMove = Math.Min(AmountToMove, c.Amount);
                }
            }
            return AmountToMove;
        }

        private double countUnitProfitForRelocationPath(LinkedList<TransportConnection> tmpList)
        {
            double UnitProfit = 0;
            for (int i = 1; i<tmpList.Count; i++)
            {
                TransportConnection c = tmpList.ElementAt(i);
                if (i % 2 == 1)
                {
                    UnitProfit += matrixD[c.X, c.Y];
                }
            }
            return UnitProfit;
        }

        private void relocateSuppliesByRelocationPath(LinkedList<TransportConnection> relocatePath)
        {
            double AmountToMove = countAmountToMoveForRelocationPath(relocatePath);
            
            for (int i = 0; i<relocatePath.Count; i++)
            {
                TransportConnection c = relocatePath.ElementAt(i);
                c.Amount += Math.Pow(-1, i) * AmountToMove;
            }
        }

        public void Calculate()
        {
            throw new System.NotImplementedException();
        }
    }
}
