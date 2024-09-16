namespace TronGame.Game_Logic
{
    // Enum to represent different types of objects in the game.
    public enum InGameObj
    {
        Void = 0, // Empty or unoccupied space.

        // PoweUps and Items. 
        Bomb = 1, // Bomb object in the game.
        Energy = 2, // Energy item - Fuel for the player object. 
        HyperVelocity = 3, // Hypervelocity power-up.
        NewJet = 4, // New JetWall - player object trail.
        Shield = 5, // Shield power-up - player object temporay invincibility.

        // Characters Representations. 
        InvincibleJetWall = 6,
        PlayerJetWall = 7,
        BotJetWall = 8,
        Invincible = 9,
        Player = 10,
        Bot = 11,
    }
}
