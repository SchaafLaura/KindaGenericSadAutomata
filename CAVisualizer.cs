namespace GenericCellular
{
    internal class CAVisualizer<T>
    {
        Func<T, ColoredGlyph> vizFunction;
        public CAVisualizer(Func<T, ColoredGlyph> vizFunction){
            this.vizFunction = vizFunction;
        }

        public ColoredGlyph Get(T state)
        {
            return vizFunction(state);
        }
    }
}
