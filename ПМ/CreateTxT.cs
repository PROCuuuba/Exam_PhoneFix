using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПМ
{
    static internal class CreateTxT
    {
        static public void CreatDirectionTxt(string names, string problem, double cost, string master_fio, string comments, string phone_name) //Создание текстового документа направления
        {

            using (StreamWriter file = new StreamWriter($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{names}-ЧЕК.txt", true))
            {
                file.Write($"Ф.И.О. клиента: {names}\r\nНеисправность: {problem}\r\nСтоимость ремонта: {cost}\r\nФ.И.О. мастера: {master_fio}\r\nКомментарий к заказу от мастера: {comments}\r\nМодель телефона: {phone_name}");
            }

            MessageBox.Show("Документ создан на рабочем столе!");

            Process.Start($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{names}-ЧЕК.txt");
        }
    }
}
