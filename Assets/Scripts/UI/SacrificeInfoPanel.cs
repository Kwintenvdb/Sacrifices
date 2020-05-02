using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SacrificeInfoPanel : MonoBehaviour
{
    //[SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private PointSystem pointSystem;
    // TODO (maybe) "stats" about sacrifice

    private System.Collections.Generic.Queue<string> messageQueue = new System.Collections.Generic.Queue<string>();

    private void Awake()
    {
        Game.Instance.SacrificeReady += OnSacrificeReady;
        Game.Instance.SacrificeThrown += OnSacrificeThrown;
    }

    private void Start()
    {
        Game.Instance.SacrificeKilled += OnSacrificeKilled;
        Game.Instance.SacrificeReleased += OnSacrificeReleased;
        Game.Instance.SacrificeMissed += OnSacrificeMissed;
    }

    // Hide panel while sacrifice is being thrown, show again when next is ready.
    private void OnSacrificeThrown(Sacrifice sacrifice)
    {
        gameObject.SetActive(false);
    }

    private void ShowKingText(string text)
    {
        // add new text to list, and coroutine works of that list
        gameObject.SetActive(true);
        if (messageQueue.Count == 0) { 
            messageQueue.Enqueue(text);
            StartCoroutine(TypeKingText());
        } else
        {
            messageQueue.Enqueue(text);
        }
    }

    private IEnumerator TypeKingText()
    {
        while (messageQueue.Count > 0)
        {
            descriptionText.text = null;

            var text = messageQueue.Peek();
            foreach (char c in text)
            {
                descriptionText.text += c;
                yield return new WaitForSeconds(0.015f);
            }
            yield return new WaitForSeconds(0.1f);
            messageQueue.Dequeue();
        }
    }

    private void OnSacrificeReady(Sacrifice sacrifice)
    {
        // Update UI info
        ShowKingText(sacrifice.Data.description);
    }

    private void OnSacrificeKilled(Sacrifice sacrifice)
    {
        print("SacrificeInfoPanel - Kill Triggered");
        ShowKingText("Another offer to the gods!");
    }

    private void OnSacrificeReleased(Sacrifice sacrifice)
    {
        print("SacrificeInfoPanel - Released Triggered");
        if (pointSystem.KingFavor > 80)
        {
            ShowKingText("Well, it was barely worth it!");
        } else if (pointSystem.KingFavor > 60)
        {
            ShowKingText("You are starting to anger me!");
        }
        else if (pointSystem.KingFavor > 40)
        {
            ShowKingText("Don't do this again! I am warning you!");
        }
        else if (pointSystem.KingFavor > 20)
        {
            ShowKingText("You are this close to unleashing my wrath upon you and your family!");
        }
        else if (pointSystem.KingFavor > 0)
        {
            ShowKingText("This was the final straw!");
        }
    }

    private void OnSacrificeMissed(Sacrifice sacrifice)
    {
        print("SacrificeInfoPanel - Miss Triggered");
        float random = UnityEngine.Random.Range(0f, 3f);
        if (random > 2f)
        {
            ShowKingText("What are you doing? Try again!");
        }
        else if (random > 1f)
        {
            ShowKingText("It's still good. Try again!");
        }
        else
        {
            ShowKingText("Don't break them! Try again!");
        }

    }
}
