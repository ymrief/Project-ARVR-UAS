using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToQuiz : InteractableObject
{
    public override void Interact()
    {
        // 1. Cek validasi standar (jarak, halangan, dll dari script induk)
        if (!isCanInteract()) return;

        // 2. Jalankan interaksi dasar (misal bunyi klik)
        base.Interact();

        // 3. PANGGIL FUNGSI LOGIKA ANDA
        MoveToQuiz();
    }

    public void MoveToQuiz()
        {
            // 1. HANCURKAN PLAYER
            // Cari object Player (biasanya pakai Tag "Player" atau Singleton)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Destroy(player);
            }

            // 2. HANCURKAN SISTEM GLOBAL (SceneSystems / UI Root)
            // Cari object SceneSystems yang dibawa dari Start awal
            GameObject systems = GameObject.Find("SceneSystems");
            if (systems != null)
            {
                Destroy(systems);
            }
            
            // Cek juga UI Root jika terpisah
            GameObject uiRoot = GameObject.Find("UIRoot");
            if (uiRoot != null)
            {
                Destroy(uiRoot);
            }

            // 3. RESET KURSOR MOUSE (PENTING!)
            // Di game (Neptunus) kursor mungkin hilang/terkunci. 
            // Di menu, kursor WAJIB muncul agar bisa klik tombol Start.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 4. PINDAH SCENE
            SceneManager.LoadScene("End");
        }
}
