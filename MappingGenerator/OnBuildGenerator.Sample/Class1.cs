﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnBuildGenerator.Sample
{
    [MappingInterface]
    public interface ISampleAutoMapperService
    {
        UserDTO MapFrom(UserEntity user);
        UserEntity MapFrom(UserDTO user);
        void Update(UserEntity entity, UserDTO dto);
        void Update(UserDTO dto, UserEntity entity);

        void DoSomethingStrange();
    }

    public class UserDTO
    {
        public string FirstName { get; }
        public string LastName { get; private set; }
        public int Age { get; set; }
        public int Cash { get; }
        public AccountDTO Account { get; private set; }
        public List<AccountDTO> Debs { get; set; }
        public UserSourceDTO Source { get; set; }
        public string Login { get; set; }
        public byte[] ImageData { get; set; }
        public List<int> LuckyNumbers { get; set; }
        public int Total { get; set; }
        public AddressDTO MainAddress { get; set; }
        public ReadOnlyCollection<AddressDTO> Addresses { get; set; }
        public int UnitId { get; set; }
        public decimal ExtraSavings { get; set; }
        public Nullable<decimal> Savings { get; set; }
        public DateTime Birthday { get; set; }
        public AuthenticationKind Authentication { get; set; }
        public string SecondAuthentication { get; set; }
        public AuthenticationKind ThirdAuthentication { get; set; }
        public int? FourthAuthentication { get; set; }
        public AuthenticationKind? FifthAuthentication { get; set; }
    }

    public class UserSourceDTO
    {
        public string ProviderName { get; set; }
        public string ProviderAddress { get; set; }
        public bool IsActive { get; set; }

        public UserSourceDTO(string providerName, string providerAddress)
        {
            ProviderName = providerName;
            ProviderAddress = providerAddress;
        }
    }

    public class AccountDTO
    {
        public string BankName { get; set; }
        public string Number { get; set; }
    }

    public class AddressDTO
    {
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string FlatNo { get; set; }
        public string BuildingNo { get; set; }
    }

    //---- Entities

    public class UserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public AccountEntity Account { get; set; }
        public List<AccountEntity> Debs { get; set; }
        public UserSourceEntity Source { get; set; }
        public string Name { get; }
        public double Cash { get; }
        public byte[] ImageData { get; set; }
        public List<int> LuckyNumbers { get; set; }
        public AddressEntity MainAddress { get; set; }
        public string AddressCity { get; set; }
        public List<AddressEntity> Addresses { get; set; }
        public UnitEntity Unit { get; set; }

        public int GetTotal()
        {
            throw new NotImplementedException();
        }

        public decimal? ExtraSavings { get; set; }
        public Nullable<decimal> Savings { get; set; }
        public DateTime Birthday { get; set; }
        public AuthenticationKind Authentication { get; set; }
        public AuthenticationKind SecondAuthentication { get; set; }
        public string ThirdAuthentication { get; set; }
        public AuthenticationKind? FourthAuthentication { get; set; }
        public int? FifthAuthentication { get; set; }
    }

    public class AddressEntity
    {
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string FlatNo { get; set; }
        public string BuildingNo { get; set; }
    }

    public class UnitEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserSourceEntity
    {
        public string ProviderName { get; set; }
        public string ProviderAddress { get; set; }
        public bool IsActive { get; set; }
    }

    public class AccountEntity
    {
        public string BankName { get; set; }
        public string Number { get; set; }
    }

    public enum AuthenticationKind
    {
        Password,
        AD
    }
}
