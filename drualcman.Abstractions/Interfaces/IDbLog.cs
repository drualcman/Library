using System;
using System.Collections.Generic;
using System.Text;

namespace drualcman.Abstractions.Interfaces
{
    public interface IDbLog
    {
        void start(string Function, string SQL, string Vars);
        void register(string Function);
        void register(string Function, string SQL);
        void register(string Function, string SQL, string Vars);
        void register(string Function, string SQL, string Vars, string info);
        void end(string Result);
        void end(object Result);
        void end(object Result, object Err);
        void end(object Result, string Err);
    }
}
