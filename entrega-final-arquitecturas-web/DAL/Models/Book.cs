using System;
using System.Collections.Generic;

namespace entrega_final_arquitecturas_web.DAL.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Category { get; set; } = null!;
}
