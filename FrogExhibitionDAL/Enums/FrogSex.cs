using System.Runtime.Serialization;

namespace FrogExhibitionDAL.Enums
{
    public enum FrogSex
    {
        [EnumMember(Value = "Male")]
        Male,

        [EnumMember(Value = "Female")]
        Female,

        [EnumMember(Value = "Hermaphrodite")]
        Hermaphrodite
    }

}
