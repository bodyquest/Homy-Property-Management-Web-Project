namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RPM.Data;
    using RPM.Data.Models;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class RentalService : IRentalService
    {
        private readonly ApplicationDbContext context;

        public RentalService(
            ApplicationDbContext context
           )
        {
            this.context = context;
        }

    }
}
