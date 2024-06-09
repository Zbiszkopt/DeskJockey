describe('Rezerwacja biurka - rezerwacja istnieje', () => {
    beforeEach(() => {

        cy.visit('http://localhost:7199/');
    });



    it('powinno otworzyć modal', () => {
        cy.contains('Rezerwacje').click();
        cy.get('input[name="Email"]').type('przykladowymail@wp.pl');
      cy.get('input[name="Password"]').type('Haslo123');
  
      cy.contains('Zaloguj!').click();
      cy.contains('Rezerwacje').click();
        cy.get('.desk[data-desk-id="1"]').click();
        

        cy.get('#reservationModal').should('be.visible');

        
    cy.get('#firstName').type('Janusz');
    cy.get('#lastName').type('Kowalski');
    cy.get('#startDate').type('2024-06-10T10:00');
    cy.get('#endDate').type('2024-06-10T18:00');
    

    cy.get('form#reservationForm').submit();

    cy.contains('No i za późno!').should('exist');
    });

    

});

