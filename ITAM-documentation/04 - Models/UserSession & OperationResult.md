---
title: "UserSession & OperationResult"
aliases:
  - UserSession
  - OperationResult
  - UserRole
tags:
  - models
  - session
  - rbac
  - result-pattern
layer: models
type: reference
updated: 2026-11-15
---

# 🔑 UserSession & OperationResult

⬅️ Kembali ke [[MOC - Layer Models]] | [[00 - Home]] | Lihat juga: [[Models.cs (Entitas & Konstanta Domain)]] · [[AuthController]] · [[Matriks Hak Akses (RBAC Admin vs Staff)]]

> [!info] Lokasi Kode Sumber
> Didefinisikan di bagian bawah file [Models/Models.cs](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L180-L245) dalam namespace `ITAssetManagement.Models`.

---

## 1. Enum `UserRole` ([Models.cs#L180-L184](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L180-L184))

Mendefinisikan dua tingkat hak akses pengguna di dalam aplikasi:
- **`UserRole.Admin`**: Memiliki hak akses penuh atas seluruh fitur (Manajemen Aset termasuk stok kondisi parsial, Manajemen Karyawan, Peminjaman & Pengembalian Alokasi, Pembersihan Riwayat, Dashboard, dan Profil).
- **`UserRole.Staff`**: Memiliki hak akses operasional (Dashboard, Peminjaman multi-unit & Pengembalian Aset pada menu Alokasi, melihat/mencari Riwayat tanpa tombol Bersihkan Riwayat, dan Profil Akun).

---

## 2. Class `UserSession` ([Models.cs#L187-L220](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L187-L220))

`UserSession` menyimpan informasi akun pengguna yang sedang login selama sesi aplikasi berlangsung menggunakan properti statis `UserSession.Current`, serta memisahkan antara **`Username` (kredensial login yang *case-sensitive*)** dan **`NamaLengkap` / `DisplayName` (nama tampilan pengguna di aplikasi)**.

| Member | Tipe | Baris | Deskripsi & Cara Kerja |
| :--- | :--- | :---: | :--- |
| `Current` | `static UserSession` | [L190](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L190) | Menyimpan referensi objek sesi pengguna yang sedang aktif. Diisi oleh [[AuthController\|AuthController.Login]] saat login sukses, dan di-reset menjadi `null` oleh [[FormLogin & FormRegister\|FormLogin.MainForm_FormClosed]] saat keluar. |
| `Username` | `string` | [L192](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L192) | Username login pengguna aktif secara *case-sensitive* (contoh: `"admin"`, `"Budi01"`). Diperbarui secara langsung jika pengguna mengganti username di [[UC_Profil]]. |
| `NamaLengkap` | `string` | [L193](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L193) | Nama pengguna / nama lengkap yang dikaitkan dengan `Username` (contoh: `"Budi Santoso"`). |
| `Role` | `UserRole` | [L194](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L194) | Role pengguna aktif (`UserRole.Admin` atau `UserRole.Staff`). |
| `DisplayName` | `string` *(get)* | [L204-L207](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L204-L207) | Mengembalikan `NamaLengkap` (atau fallback ke `Username` jika kosong). Digunakan untuk menampilkan nama pengguna di header `MainForm` serta sebagai pencatat pelaku (`oleh_user`) di [[Tabel riwayat]]. |
| `IsAdmin` | `bool` *(get)* | [L210-L213](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L210-L213) | Mengembalikan `Role == UserRole.Admin`. Digunakan oleh UI untuk mengatur visibilitas kontrol khusus Admin. |
| `ProfileText` | `string` *(get)* | [L216-L219](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L216-L219) | Menghasilkan string `"<DisplayName> (Admin IT)"` atau `"<DisplayName> (Staff IT)"` yang ditampilkan pada label `lblProfil` di pojok kanan atas [[MainForm (Navigasi & RBAC UI)\|MainForm]]. |

---

## 3. Class `OperationResult` ([Models.cs#L223-L245](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Models/Models.cs#L223-L245))

`OperationResult` menerapkan **Result Pattern** sebagai kontrak komunikasi standar antara layer **Controllers** dan layer **Views**.

```csharp
public class OperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public OperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static OperationResult Ok(string message) => new OperationResult(true, message);
    public static OperationResult Fail(string message) => new OperationResult(false, message);
}
```

### Keunggulan Penggunaan `OperationResult`:
1. **Dekople penuh antara Controller dan UI**: Controller tidak perlu memanggil `MessageBox.Show`, sehingga mudah diuji secara independen dari proyek pengujian eksternal.
2. **Integrasi Satu Baris dengan `Dialogs.Show(result)`**: Di layer Views, hasil dari Controller cukup dilewatkan ke `Dialogs.Show(result)` ([Dialogs.cs#L36-L46](file:///d:/AMIKOM/Pemrograman%20Lanjut/Project/ITAssetManagement/ITAssetManagement/Views/Dialogs.cs#L36-L46)) yang otomatis memilih `Dialogs.Info` (ikon informasi) jika `Success == true` atau `Dialogs.Warn` (ikon peringatan) jika `Success == false`.
