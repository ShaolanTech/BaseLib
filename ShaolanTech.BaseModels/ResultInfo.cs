using System;

namespace ShaolanTech.BaseModels
{

    /// <summary>
    /// Result container for invoking a method or an api, this type is used when this method or api has no return value
    /// </summary>
    public class ResultInfo
    {

        private bool operationDone;

        /// <summary>
        /// Whether this call is successful or not
        /// </summary>
        public bool OperationDone
        {
            get { return operationDone; }
            set
            {
                operationDone = value;
                if (!value)
                {
                    this.LogTime = DateTime.Now;

                }
            }
        }

        public DateTime LogTime { get; set; }

        private string message;

        /// <summary>
        /// Message when this call is failed
        /// </summary>
        public string Message
        {
            get { return message; }
            set
            {
                
                message = value;

            }
        }

        public int Code { get; set; }

        public ResultInfo()
        {
            this.OperationDone = true;
            this.LogTime = DateTime.Now;
            this.message = "";
        } 
    }
    /// <summary>
    /// Result container for invoking a method or an api, this type is used when this method or api has return value, and you need fill "AdditionalData" property before returning.
    /// </summary>
    public class ResultInfo<T> : ResultInfo
    {

        public T AdditionalData { get; set; }
    }

}
