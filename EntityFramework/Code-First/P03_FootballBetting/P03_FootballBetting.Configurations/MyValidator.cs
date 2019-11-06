namespace P03_FootballBetting.Configurations
{
    public class MyValidator
    {
        public class Color
        {
            public const int NameLength = 20;
        }

        public class Country
        {
            public const int NameLength = 50;
        }

        public class Player
        {
            public const int NameLength = 100;
        }

        public class Position
        {
            public const int NameLength = 10;
        }

        public class Team
        {
            public const int NameLength = 25;
            public const int LogoUrlLength = 250;
            public const int InitialsLength = 3;
        }

        public class Town
        {
            public const int NameLength = 20;
        }

        public class User
        {
            public const int NameLength = 25;
            public const int UsernameLength = 20;
            public const int PasswordLength = 30;
            public const int EmailLength = 50;
        }
    }
}
