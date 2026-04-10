using System;
using System.Collections.Generic;
using System.Text;

namespace FileConsistencyManager.Models
{
    internal class CustomMessageBoxTypes
    {
        public enum CustomMessageBoxButtons
        {
            OK,
            OKCancel,
            YesNo,
            YesNoCancel
        }

        public enum CustomMessageBoxIcon
        {
            None,
            Information,
            Warning,
            Error
        }

        public enum CustomDialogResult
        {
            None,
            OK,
            Cancel,
            Yes,
            No
        }
    }
}
