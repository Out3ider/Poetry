using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// �ֵ���ӿ�
    /// </summary>
    public interface IDict : IModel
    {
        int Id { set; get; }

        string Name { set; get; }

        decimal OrderByNo { set; get; }
    }
}