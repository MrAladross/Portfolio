struct ByteWord
{
    public int Number { get; }
    public string Word { get; }

    public ByteWord(int num, string wor)
    {
        Number = num;
        Word = wor;
    }
    
    public override string ToString()
    {
        return Word + " " + Number;
    }

}
