﻿namespace EUN.Entity.Response
{
    using EUN.Common;

    public class LeaveLobbyOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public LeaveLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
#if EUN
            if (!HasError)
            {
                Success = true;
            }
#endif
        }
    }
}