using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseInMemoryDatabase("ToDoDb"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/todo",async (AppDbContext context) => {
    var items = await context.ToDoItems.ToListAsync();
    return Results.Ok(items);
});

app.MapPost("api/todo",async (AppDbContext context, ToDoItem item) => {
    await context.ToDoItems.AddAsync(item);
    await context.SaveChangesAsync();
    return Results.Created($"/api/todo/{item.Id}",item);
}); 

app.Run();


