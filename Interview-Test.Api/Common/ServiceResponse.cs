namespace Interview_Test.Common;

// Standard response wrapper — ใช้ครอบทุก response จาก Service
// ทำให้ frontend รู้สถานะได้จาก body โดยไม่ต้องอ่าน HTTP status เท่านั้น
// Data = ข้อมูลจริง, Success = สำเร็จหรือไม่, Message = ข้อความ (OK / error message)
public class ServiceResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}
