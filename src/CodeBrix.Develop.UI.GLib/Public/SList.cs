using System.Collections;
using System.Collections.Generic;

namespace CodeBrix.Develop.UI.GLib; //was previously: GLib;

public partial class SList : IEnumerable<SListElement>
{
    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator<SListElement> IEnumerable<SListElement>.GetEnumerator()
    {
        return new Enumerator(this);
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }
}
