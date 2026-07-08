namespace GoPowered.Lang.Parser
{
    using TokenMeta = Dictionary<string, object>;

    public interface IParserToken
    {
        private static readonly Dictionary<IParserToken, TokenMeta> Metas = [];

        public TokenMeta Meta {
            get {
                if (!Metas.ContainsKey(this))
                {
                    var meta = new TokenMeta();
                    Metas[this] = meta;

                    return meta;
                }

                return Metas[this];
            }
        }

        public static void CleanMetas()
        {
            Metas.Clear();
        }
    }
}
