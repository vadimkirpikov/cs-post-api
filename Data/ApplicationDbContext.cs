﻿using Microsoft.EntityFrameworkCore;

namespace CsPostApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
{
    
}