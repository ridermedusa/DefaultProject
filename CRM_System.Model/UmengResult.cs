using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_System.Model
{
    public class UmengResult
    {
        //返回结果，"SUCCESS"或者"FAIL"
        private string _ret;
        private string _error_code;
        private string _msg_id;
        private string _task_id;
        private string _thirdparty_id;

        public string Ret
        {
            get
            {
                return _ret;
            }

            set
            {
                _ret = value;
            }
        }

        public string Error_code
        {
            get
            {
                return _error_code;
            }

            set
            {
                _error_code = value;
            }
        }

        public string Msg_id
        {
            get
            {
                return _msg_id;
            }

            set
            {
                _msg_id = value;
            }
        }

        public string Task_id
        {
            get
            {
                return _task_id;
            }

            set
            {
                _task_id = value;
            }
        }

        public string Thirdparty_id
        {
            get
            {
                return _thirdparty_id;
            }

            set
            {
                _thirdparty_id = value;
            }
        }
    }
}
