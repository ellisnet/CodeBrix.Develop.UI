using System;

namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

internal class IncludeFactory
{
    public Include Create(Input.Include include)
    {
        if (include.Name is null || include.Version is null)
            throw new Exception("Can't create include because data is missing");

        return new Include(include.Name, include.Version);
    }
}
