namespace TitanFitenss.Domain.TrainerAggregate;
    public class Trainer
    {
    public int TrainerId{get;private set;}
    public string TrainerName{get;private set;}=null!; 
    public string Email{get;private set;}=null!;
    public string Phone{get;private set;}=null!;     
    public bool IsActive{get;private set;}           
    private Trainer(){}
    public Trainer(string trainerName, string email, string phone, bool isActive=true)
    {
        if (string.IsNullOrWhiteSpace(trainerName))
            throw new ArgumentException("Trainer name is required",nameof(trainerName));
        TrainerName=trainerName;
        Email=email;
        Phone=phone;
        IsActive=isActive;
    }
    public void UpdateDetails(string trainerName, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(trainerName))
            throw new ArgumentException("Trainer name is required",nameof(trainerName));
        TrainerName=trainerName;
        Email=email;
        Phone=phone;
    }
    public void Activate()
    {
        IsActive=true;
    }
    public void Deactivate()
    {
        IsActive=false;
    }
    }