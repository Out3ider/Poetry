using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// ×ÖµäÀà½Ó¿Ú
    /// </summary>
    public interface IDict : IModel
    {
        int Id { set; get; }

        string Name { set; get; }

        decimal OrderByNo { set; get; }
    }
}