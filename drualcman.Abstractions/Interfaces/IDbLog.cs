using System;
using System.Collections.Generic;
using System.Text;

namespace drualcman.Abstractions.Interfaces
{
    /// <summary>
    /// This class is use for tracking
    /// </summary>
    public interface IDbLog
    {
        /// <summary>
        /// Start the transaction.
        /// </summary>
        /// <param name="Function">Method or function name to be called</param>
        /// <param name="SQL">sql query to be used</param>
        /// <param name="Vars">Variables passed on the method or function or variables that needed to be tracked. You can put many variables here by concatinating them on a string</param>
        void start(string Function, string SQL, string Vars);

        /// <summary>
        /// Track something in the middle of the transaction
        /// </summary>
        /// <param name="Function">Function or method name to be called</param>
        void register(string Function);

        /// <summary>
        /// Track something in the middle of the transaction
        /// </summary>
        /// <param name="Function">Function or method name to be called</param>
        /// <param name="SQL">sql query to be used</param>
        void register(string Function, string SQL);

        /// <summary>
        /// Track something in the middle of the transaction
        /// </summary>
        /// <param name="Function">Function or method name to be called</param>
        /// <param name="SQL">sql query to be used</param>
        /// <param name="Vars">Variables passed on the method or function or variables that needed to be tracked. You can put many variables here by concatinating them on a string</param>
        void register(string Function, string SQL, string Vars);

        /// <summary>
        /// Track something in the middle of the transaction
        /// </summary>
        /// <param name="Function">Function or method name to be called</param>
        /// <param name="SQL">sql query to be used</param>
        /// <param name="Vars">Variables passed on the method or function or variables that needed to be tracked. You can put many variables here by concatinating them on a string</param>
        /// <param name="info">Additional info need to track</param>
        void register(string Function, string SQL, string Vars, string info);

        /// <summary>
        /// End of tracking
        /// </summary>
        /// <param name="Result">Result to be returned from the method or function or from a sql query, Can use multiple results and concatinate them using string</param>
        void end(string Result);

        /// <summary>
        /// End of tracking
        /// </summary>
        /// <param name="Result">Result to be returned from the method or function or from a sql query</param>
        void end(object Result);

        /// <summary>
        /// End of tracking
        /// </summary>
        /// <param name="Result">Result to be returned from the method or function or from a sql query</param>
        /// <param name="Err">Exception to throw when there is errors or any message or variable you want to track together with the result</param>
        void end(object Result, object Err);

        /// <summary>
        /// End of tracking
        /// </summary>
        /// <param name="Result">Result to be returned from the method or function or from a sql query</param>
        /// <param name="Err">Exception to throw when there is errors or any message or variable you want to track together with the result. Can use multiple variables by concatinating using string</param>
        void end(object Result, string Err);
    }
}
