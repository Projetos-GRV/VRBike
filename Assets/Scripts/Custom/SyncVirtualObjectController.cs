using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncVirtualObjectController : MonoBehaviour
{
    [Header("Pontos de refer�ncia do objeto real (m�os detectadas)")]
    public Transform realPointA; // ex: m�o esquerda
    public Transform realPointB; // ex: m�o direita

    [Header("Pontos correspondentes no objeto virtual")]
    public Transform virtualPointA; // ex: ponto equivalente � m�o esquerda no prefab
    public Transform virtualPointB; // ex: ponto equivalente � m�o direita no prefab

    [Header("Objeto virtual a alinhar")]
    public Transform virtualObject;

    public void UpdateBikePosition()
    {
        // --- POSI��O ---
        // Ponto m�dio dos pontos reais
        Vector3 realMid = (realPointA.position + realPointB.position) / 2f;
        // Ponto m�dio dos pontos virtuais
        Vector3 virtualMid = (virtualPointA.position + virtualPointB.position) / 2f;

        // Ajusta posi��o
        Vector3 positionOffset = realMid - virtualMid;
        virtualObject.position += positionOffset;

        // --- ROTA��O (apenas eixo Y) ---
        // Dire��o real no plano XZ
        Vector3 realDir = realPointB.position - realPointA.position;
        realDir.y = 0; // ignora inclina��o

        // Dire��o virtual no plano XZ
        Vector3 virtualDir = virtualPointB.position - virtualPointA.position;
        virtualDir.y = 0; // ignora inclina��o

        if (realDir.sqrMagnitude > 0.0001f && virtualDir.sqrMagnitude > 0.0001f)
        {
            // Calcula rota��o apenas no Y
            Quaternion targetRot = Quaternion.FromToRotation(virtualDir.normalized, realDir.normalized);

            // Extrai apenas a componente de Y
            Vector3 euler = (targetRot * virtualObject.rotation).eulerAngles;
            virtualObject.rotation = Quaternion.Euler(0, euler.y, 0);
        }
    }

}
