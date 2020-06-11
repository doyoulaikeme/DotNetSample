using easyMVCFramework.网络请求处理类;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyMVCFramework.自定义控制器接口
{
    public interface IController
    {
        void Exec(HttpContextWrapper context);
    }
}
