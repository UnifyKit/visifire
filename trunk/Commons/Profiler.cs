/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
*/



using System;
using System.Diagnostics;
using System.Reflection;

namespace Visifire.Commons
{   
    /// <summary>
    /// Used for application profiling 
    /// </summary>
    public class Profiler
    {

        #region "Public Method" 
            
            public Profiler()
            {
                _totalTimeSpan = new TimeSpan(0, 0, 0);

                _previousMark = new TimeSpan(System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second, System.DateTime.Now.Millisecond);

                _marks = new System.Collections.Generic.Dictionary<string, TimeSpan>();
            }

            /// <summary>
            /// Collect start time stamp
            /// </summary>
            public void Start()
            {
               _startTimeSpan = new TimeSpan(System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second, System.DateTime.Now.Millisecond);
            }

            /// <summary>
            /// Collect end time stamp
            /// </summary>
            public void End()
            {
                _endTimeSpan = new TimeSpan(System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second, System.DateTime.Now.Millisecond);
            }

            public void Mark(String msg)
            {
                TimeSpan currentMark = new TimeSpan(System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second, System.DateTime.Now.Millisecond);

                _marks.Add(msg, currentMark.Subtract(_previousMark));

                _totalTimeSpan = _totalTimeSpan.Add(currentMark.Subtract(_previousMark));

                _previousMark = currentMark;
            }

            /// <summary>
            ///  Overloaded function Log the reports in logger 
            /// </summary>
            public void LogReports(String LoggerDivId)
            {
                Logger.TargetID = LoggerDivId;
                BuildReport();
                Logger.Log(_reports);
            }

            /// <summary>
            ///  Overloaded function Log the reports in logger with a message
            /// </summary>
            public void LogReports(String LoggerDivId, String message)
            {
                Logger.TargetID = LoggerDivId;
                BuildReport();
                Logger.Log("\n" + message + _reports);
            }

            /// <summary>
            ///  Overloaded function Log the reports in logger with a message
            /// </summary>
            public void LogReports(String LoggerDivId, Boolean miniReport)
            {
                Logger.TargetID = LoggerDivId;

                if (!miniReport)
                {
                    
                    BuildReport();
                    Logger.Log(_reports);
                }
                else
                {
                    System.Collections.Generic.Dictionary<String, TimeSpan>.Enumerator enumerator;

                    enumerator = _marks.GetEnumerator();

                    for (int i = 0; i < _marks.Count; i++)
                    {
                        enumerator.MoveNext();

                        TimeSpan ts = enumerator.Current.Value;
                        String msg = enumerator.Current.Key;

                        Logger.Log("\n" + msg + " :  " +
                                "Time Taken : " + ts.ToString() + "   " +
                                "Total Time : " + _totalTimeSpan.ToString() + "   " +
                                "Percentage Time :" + ((Double)ts.Ticks / (Double)_totalTimeSpan.Ticks * 100).ToString());
                    }
                }
            }
        
            /// <summary>
            /// Log the reports in default logger 
            /// </summary>
            public void LogReports()
            {
                Logger.TargetID = "Logger";
                BuildReport();
                Logger.Log(_reports);
            }

            /// <summary>
            /// Get report
            /// </summary>
            /// <returns>Returns a string</returns>
            private void BuildReport()
            {
                
                _reports = "\n--------------\nFunctionInfo:\n" 
                         + FunctionInfo() 
                         +"\n--------------" ;

                _reports += "\nStartsAt " + _startTimeSpan.ToString() +
                          "\nEndsAt: " + _endTimeSpan.ToString() + 
                          "\nTimeDiff:" + TimeDiff();
            }

        #endregion

        #region "Private Method"

            /// <summary>
            /// Calculates the time difference
            /// </summary>
            private String TimeDiff()
            {
                return (_endTimeSpan.Subtract(_startTimeSpan).ToString());
            }

            /// <summary>
            /// Collect information about function call
            /// </summary>
            private String FunctionInfo()
            {
                string revVal;          // Rerurn msg     
                string info = "";       // Function information string

                // Extract Information from stack trace using reflection
                StackTrace stackTrace = new System.Diagnostics.StackTrace();
                StackFrame stackFrame = stackTrace.GetFrame(3);
                MethodBase methodBase = stackFrame.GetMethod();
                ParameterInfo[] parInfo = methodBase.GetParameters();

                // Get parameter information
                foreach (System.Reflection.ParameterInfo pInfo in parInfo)
                {
                    info += pInfo.ToString();
                }

                // Construct information  as return value
                revVal = methodBase.DeclaringType.FullName + "." + methodBase.Name + "(" + info.ToString() + ")";

                return revVal;
            }

        #endregion

        #region "Data"

        private TimeSpan _startTimeSpan;        // Start time
        private TimeSpan _endTimeSpan;          // End time
        private TimeSpan _totalTimeSpan;
        private String _reports;                // Report information

        private TimeSpan _previousMark;
        private System.Collections.Generic.Dictionary<String, TimeSpan> _marks;

        #endregion

    }
}
