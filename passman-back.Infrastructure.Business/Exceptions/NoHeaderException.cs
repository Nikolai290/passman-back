using System;

namespace passman_back.Infrastructure.Business.Exceptions {
    public class NoHeaderException : Exception {

        public NoHeaderException(string message) : base(message) { }
    }
}
