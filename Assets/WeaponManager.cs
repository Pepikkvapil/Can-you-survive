using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum WeaponType
    {
        BaseWeapon,
        SplitBaseWeapon,
        PierceWeapon // Add more weapon types as needed
    }

    [SerializeField]
    private WeaponType selectedWeapon = WeaponType.BaseWeapon; // Selected weapon at the start

    [SerializeField]
    private GameObject baseWeapon;

    [SerializeField]
    private GameObject splitBaseWeapon;

    [SerializeField]
    private GameObject pierceWeapon;

    private GameObject currentWeapon;

    [SerializeField]
    private GameObject weaponSelectionCanvas; // Reference to the canvas for weapon selection

    void Start()
    {
        OpenWeaponSelection();
        // Deactivate all weapons at the start
        DeactivateAllWeapons();
    }

    // Add a public method to open the weapon selection UI
    public void OpenWeaponSelection()
    {
        weaponSelectionCanvas.SetActive(true);
    }

    // Add a public method to close the weapon selection UI
    public void CloseWeaponSelection()
    {
        weaponSelectionCanvas.SetActive(false);
    }

    // Add a public method to select a weapon at the start
    public void SelectWeapon(string weaponType)
    {
        switch (weaponType)
        {
            case "BaseWeapon":
                selectedWeapon = WeaponType.BaseWeapon;
                break;
            case "SplitBaseWeapon":
                selectedWeapon = WeaponType.SplitBaseWeapon;
                break;
            case "PierceWeapon":
                selectedWeapon = WeaponType.PierceWeapon;
                break;
                // Add cases for other weapons if needed
        }

        // Activate the selected weapon immediately
        DeactivateAllWeapons();
        ActivateSelectedWeapon();
        CloseWeaponSelection();
    }

    // Helper method to activate the selected weapon
    private void ActivateSelectedWeapon()
    {
        // Deactivate the current weapon
        DeactivateAllWeapons();

        // Activate the selected weapon
        switch (selectedWeapon)
        {
            case WeaponType.BaseWeapon:
                baseWeapon.SetActive(true);
                currentWeapon = baseWeapon;
                break;
            case WeaponType.SplitBaseWeapon:
                splitBaseWeapon.SetActive(true);
                currentWeapon = splitBaseWeapon;
                break;
            case WeaponType.PierceWeapon:
                pierceWeapon.SetActive(true);
                currentWeapon = pierceWeapon;
                break;
                // Add cases for other weapons if needed
        }
    }

    // Helper method to deactivate all weapons
    private void DeactivateAllWeapons()
    {
        baseWeapon.SetActive(false);
        splitBaseWeapon.SetActive(false);
        pierceWeapon.SetActive(false);
        // Deactivate other weapons if needed
    }
}
