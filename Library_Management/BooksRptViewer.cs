using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library_Management
{
    public partial class BooksRptViewer : Form
    {

        List<Book> _list;
        public BooksRptViewer(List<Book> booksrpt)
        {
            InitializeComponent();
            _list = booksrpt;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void BooksReportViewer_Load(object sender, EventArgs e)
        {
            BooksReport rpt = new BooksReport();
            rpt.SetDataSource(_list);
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Refresh();
        }
    }
}
