using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;
using Backend.Services.PlayerServices.Interfaces;

namespace Backend.Services.PlayerServices

{
    public class PlayerService : IPlayerService
    {
        Piece IPlayerService.GetBasePiece()
        {
            throw new NotImplementedException();
        }

        //Player IPlayerService.SelectPiece(int pieceId)
        //{
        //    throw new NotImplementedException();
        //}

        public Piece SelectPiece(Player player, int pieceId)
        {
            var selectedPiece = player.Pieces.FirstOrDefault(p => p.ID == pieceId);

            return selectedPiece;
        }
    }
}
