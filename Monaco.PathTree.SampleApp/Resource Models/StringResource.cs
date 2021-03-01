namespace Monaco.PathTree.SampleApp
{
    public class StringResource : Resource
    {
        public string Contents { get; set; }

        public StringResource(string name, string contents)
        {
            Name = name;
            Contents = contents;
        }
    }
}
