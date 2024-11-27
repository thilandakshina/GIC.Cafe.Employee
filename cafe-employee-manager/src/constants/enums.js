export const GenderType = {
    Male: 0,
    Female: 1,
    Other: 2
  };
  
  export const getGenderLabel = (value) => {
    switch (value) {
      case GenderType.Male:
        return 'Male';
      case GenderType.Female:
        return 'Female';
      case GenderType.Other:
        return 'Other';
      default:
        return '';
    }
  };