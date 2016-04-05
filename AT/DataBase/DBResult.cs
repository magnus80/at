using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AT.Global;
using NUnit.Framework;

namespace AT.DataBase
{
    /// <summary>
    /// Класс хранения результатов выборки из БД
    /// </summary>
    public class DBResult
    {
        internal List<List<string>> Rows = new List<List<string>>();



        public int Count
        {
            get { return Rows.Count; }
        }

        #region indexators

        public string this[int row, int col]
        {
            get
            {
                return Rows[row][col];
               // return GetCellFromRow(row, col);
            }
        }

        #endregion

        
        /// <summary>
        /// Возвращает любое значние из столбца
        /// </summary>
        /// <param name="column">Номер столбца</param>
        public string GetAnyCellFromColumn(int column)
        {
            return Rows[new Random().Next(0, Rows.Count - 1)][column];
        }

        /// <summary>
        /// Возвращает первое значение в указанном стобле
        /// </summary>
        /// <param name="column">Номер столбца</param>
        public string GetFirstCellFromColumn(int column)
        {
            return Rows[0][column];
        }

        /// <summary>
        /// Возвращает все ячейки из указанного столбца
        /// </summary>
        /// <param name="column">номер столбца</param>
        public List<string> GetAllCellsFromColumn(int column)
        {
            try
            {
                var list = new List<string>();
                for (int i = 0; i < Rows.Count; i++)
                {
                    list.Add(Rows[i][column]);
                }
                return list;
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }
        }

        /// <summary>
        /// Возвращает все ячейки рандомной строки выборки, в порядке, который был указан в операторе SELECT
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCellsFromAnyRow()
        {
            return Rows[new Random().Next(0, Rows.Count - 1)];
        }

        /// <summary>
        /// Возвращает все ячейки рандомной указанной стрки выборки, в порядке, который был указан в операторе SELECT
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCellsFromRow(int row)
        {
            try
            {
                return Rows[row];
            }
            catch (Exception ex)
            {
                GlobalEvents.ExeptionFounded(ex);
                return null;
            }

        }
    }
}
