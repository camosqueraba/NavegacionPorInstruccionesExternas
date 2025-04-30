namespace NavegacionPorInstruccionesExternas.DTOs
{    
    public class ResultDownloadedFileDTO
    {
        #region Propiedades
        public bool IsDownloadCompleted { get; set; }
        public bool IsDownloadStarted { get; set; }
        public bool IsOnDirectory { get; set; }
        public bool ErrorExist { get; set; }
        public string ErrorMessage { get; set; }
        public string FileName { get; set; }
        #endregion
    }    
}