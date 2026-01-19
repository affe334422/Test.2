
using Microsoft.Xna.Framework;

public class MinRectangle : QT_Compatible
{
    protected Rectangle rectangle;
    protected Vector2 Centrum;
    public MinRectangle(int x, int y, int width,int height)
    {
        rectangle=new Rectangle(x-(width/2),y-(height/2),width,height);
        Centrum=new Vector2(x,y);
    }
    public MinRectangle(Vector2 xy,int width,int height)
    {
        rectangle=new Rectangle((int)xy.X-(width/2),(int)xy.Y-(height/2),width,height);
        Centrum=xy;
    }
    public float centrum_x
    {
        get => centrum.X; set
        {
            Centrum.X=value;
            rectangle.X=(int)Centrum.X-rectangle.Width/2;
        }
    }
    public float centrum_y
    {
        get => Centrum.Y; set
        {
            Centrum.Y=value;
            rectangle.Y=(int)Centrum.Y-rectangle.Height/2;
        }
    }
    public Rectangle rec{get=>rectangle;}
    public Vector2 centrum{
        get=>Centrum;
        set{
            Centrum=value;
            rectangle.X=(int)(Centrum.X-rectangle.Width/2);
            rectangle.Y=(int)(Centrum.Y-rectangle.Height/2);
        }
    }
    Vector2 QT_Compatible.CentrumXY()
    {
        return centrum;
    }
    Rectangle QT_Compatible.GetRectangle()
    {
        return rec;
    }
    MinRectangle QT_Compatible.GetMinRectangle()
    {
        throw new System.NotImplementedException();
    }

}
