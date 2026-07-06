using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Fixer.Record;

namespace CodeBrix.Develop.UI.Generator.Fixer; //was previously: Generator.Fixer;

public static class Records
{
    private static readonly List<Fixer<GirModel.Record>> Fixers =
    [
        new DisableBrokenTypes(),
        new PropertyNamedLikeRecordFixer(),
        new InternalMethodsNamedLikeRecordFixer(),
        new MethodWithInOutInstanceParameterFixer(),
        new PublicMethodsColldingWithFieldFixer(),
        new RecordEqualsMethodCollidesWithGeneratedCode(),
        new PublicMethodsWithCallbackReturnWhichIsFundamental()
    ];

    public static void Fixup(IEnumerable<GirModel.Record> records)
    {
        foreach (var record in records)
            foreach (var fixer in Fixers)
                fixer.Fixup(record);
    }
}
