﻿using MediatR;

namespace Catalog.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int Id) : IRequest;
}
