namespace SeboOnline.Models;

// Feito baseado no seguinte repositório: https://github.com/balta-io/2808/
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IList<User> Users { get; set; }
}