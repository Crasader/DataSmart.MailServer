﻿using System;
using System.Collections.Generic;
using System.Text;

namespace System.NetworkToolkit.IMAP
{
    /// <summary>
    /// This class represents FETCH request RFC822.TEXT argument(data-item). Defined in RFC 3501.
    /// </summary>
    public class IMAP_t_Fetch_i_Rfc822Text : IMAP_t_Fetch_i
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public IMAP_t_Fetch_i_Rfc822Text()
        {
        }


        #region override method ToString

        /// <summary>
        /// Returns this as string.
        /// </summary>
        /// <returns>Returns this as string.</returns>
        public override string ToString()
        {
            return "RFC822.TEXT";
        }

        #endregion
    }
}
