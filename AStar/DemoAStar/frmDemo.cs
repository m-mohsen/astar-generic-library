/* Copyright 2011 M. Mohsen (Email: m.mohsen.mahmoud@gmail.com)
 * 
 * This file is part of AStar.
 * 
 * AStar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * AStar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with AStar.  If not, see <http://www.gnu.org/licenses/>.
 * */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AStar;

namespace DemoAStar
{
    public partial class frmDemo : Form
    {
        private const string emptyTag = "Empty";
        private const string obstacleTag = "Obstacle";
        private const string initialTag = "Initial";
        private const string goalTag = "Goal";
        private const string routeTag = "Route";
        private const int width = 9;
        private const int height = 5;

        public frmDemo()
        {
            InitializeComponent();
        }

        private void frmDemo_Load(object sender, EventArgs e)
        {
            helpProvider.SetHelpString(pnlMAP, "Click on a cell to change its state.");
            foreach (Control cell in pnlMAP.Controls)
            {
                helpProvider.SetHelpString(cell, "Click on a cell to change its state.");
                MakeEmpty(cell as Button);
                cell.Click += new EventHandler(Cell_Click);
            }
        }

               
        private void Cell_Click(object sender, EventArgs e)
        {
            Button btnCell = sender as Button;

            switch ((string)btnCell.Tag)
            {
                case emptyTag:
                    MakeObstacle(btnCell);
                    break;

                case obstacleTag:
                    MakeInitial(btnCell);
                    break;

                case initialTag:
                    MakeGoal(btnCell);
                    break;

                case goalTag:
                case routeTag:
                    MakeEmpty(btnCell);
                    break;
            }
        }

        #region Cells States
        private void MakeObstacle(Button cell)
        {
            cell.BackColor = Color.LightGray;
            cell.Tag = obstacleTag;
            cell.Text = obstacleTag;
        }

        private void MakeInitial(Button cell)
        {
            cell.BackColor = Color.LightPink;
            cell.Tag = initialTag;
            cell.Text = initialTag;
        }

        private void MakeGoal(Button cell)
        {
            cell.BackColor = Color.LightBlue;
            cell.Tag = goalTag;
            cell.Text = goalTag;
        }

        private void MakeEmpty(Button cell)
        {
            cell.BackColor = Color.Transparent;
            cell.Tag = emptyTag;
            cell.Text = "";
        }

        private void MakeRoute(Button cell)
        {
            cell.BackColor = Color.LightGreen;
            cell.Tag = routeTag;
            cell.Text = "";
        }
        #endregion

        private void btnFindRoute_Click(object sender, EventArgs e)
        {
            RouteFinder routeFinder = new RouteFinder(0, 0, width-1, height-1, chbCanMoveDiagonally.Checked);

            int countInitialLocations = 0;
            int countGoalLocations = 0;
            for (int i = 0; i < pnlMAP.Controls.Count; i++)
            {
                int x = i % width;
                int y = i / width;

                Location loc = new Location(x, y);

                switch ((string)pnlMAP.Controls[i].Tag)
                {
                    case initialTag:
                        countInitialLocations++;
                        if (countInitialLocations > 1)
                        {
                            MessageBox.Show("There cannot be more than one initial location.", this.Text);
                            return;
                        }
                        routeFinder.InitialLocation = loc;
                        break;

                    case obstacleTag:
                        routeFinder.AddObstacle(loc);
                        break;

                    case goalTag:
                        countGoalLocations++;
                        routeFinder.AddGoal(loc);
                        break;

                    case routeTag:
                        MakeEmpty(pnlMAP.Controls[i] as Button);
                        break;
                }
            }

            if (countGoalLocations == 0)
            {
                MessageBox.Show("There must be atleast one goal location.", this.Text);
            }

            DisplayRoute(routeFinder.CalculateRoute());
        }

        private void DisplayRoute(List<Location> route)
        {
            if (route != null)
            {
                int size = route.Count - 1;
                for (int i = 0; i < size; i++)
                {
                    int x = route[i].X;
                    int y = route[i].Y;

                    MakeRoute(pnlMAP.Controls[width * y + x] as Button);
                }
            }
            else
            {
                MessageBox.Show("There is no route between the initial location and a goal location.",this.Text);
            }
        }
    }
}
