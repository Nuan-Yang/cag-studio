using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAG.Attributes
{
    /// <summary>
    /// 所有需要暴露给js的dotnet类应添加该特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Dotnet2JSAttribute : Attribute
    {
        public Dotnet2JSAttribute()
        {
            
        }


    }
}
