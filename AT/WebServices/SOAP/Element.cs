
namespace AT.WebServices.SOAP
{
    internal class Element
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string ParentName { get; set; }

        public int ParentId { get; set; }

        public int Id { get; set; }

        public Element(string name, int parent_id, string value = "")
        {
            Value = value;
            ParentId = parent_id;
            ParentName = parent_id == -1 ? "" : SoapParamList.GetElementName(parent_id);
            Id = SoapParamList.Count++;

            Name = name + "__" + Id;
        }
    }
}
