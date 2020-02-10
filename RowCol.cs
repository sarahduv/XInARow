namespace XInARow
{
    class RowCol
    {
        public int Row;
        public int Col;

        public RowCol(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool isValid(int max)
        {
            return Row >= 0 && Col >= 0 && Row < max && Col < max;
        }
    }
}
