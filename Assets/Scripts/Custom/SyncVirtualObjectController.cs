using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncVirtualObjectController : MonoBehaviour
{
    [Header("Pontos de referência do objeto real (mãos detectadas)")]
    public Transform realPointA; // ex: mão esquerda
    public Transform realPointB; // ex: mão direita

    [Header("Pontos correspondentes no objeto virtual")]
    public Transform virtualPointA; // ex: ponto equivalente à mão esquerda no prefab
    public Transform virtualPointB; // ex: ponto equivalente à mão direita no prefab

    [Header("Objeto virtual a alinhar")]
    public Transform virtualObject;

    public void UpdateBikePosition()
    {
        // --- POSIÇÃO ---
        // Ponto médio dos pontos reais
        Vector3 realMid = (realPointA.position + realPointB.position) / 2f;
        // Ponto médio dos pontos virtuais
        Vector3 virtualMid = (virtualPointA.position + virtualPointB.position) / 2f;

        // Ajusta posição
        Vector3 positionOffset = realMid - virtualMid;
        virtualObject.position += positionOffset;

        // --- ROTAÇÃO (apenas eixo Y) ---
        // Direção real no plano XZ
        Vector3 realDir = realPointB.position - realPointA.position;
        realDir.y = 0; // ignora inclinação

        // Direção virtual no plano XZ
        Vector3 virtualDir = virtualPointB.position - virtualPointA.position;
        virtualDir.y = 0; // ignora inclinação

        if (realDir.sqrMagnitude > 0.0001f && virtualDir.sqrMagnitude > 0.0001f)
        {
            // Calcula rotação apenas no Y
            Quaternion targetRot = Quaternion.FromToRotation(virtualDir.normalized, realDir.normalized);

            // Extrai apenas a componente de Y
            Vector3 euler = (targetRot * virtualObject.rotation).eulerAngles;
            virtualObject.rotation = Quaternion.Euler(0, euler.y, 0);
        }
    }

}
