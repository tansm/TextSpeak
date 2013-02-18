using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnglishReader {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.Run(new FormMain());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            if (!(e.Exception is OperationCanceledException)) {
                System.Windows.Forms.MessageBox.Show(e.Exception.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

    }
}
