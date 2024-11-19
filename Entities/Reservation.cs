using System;
using System.Collections.Generic;

namespace ApiBooks.Entities;

public partial class Reservation
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime ReservationDate { get; set; }

    public DateTime ReturnDate { get; set; }

    public virtual Book Book { get; set; } = null!;
}
